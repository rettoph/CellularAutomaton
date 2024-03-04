using CellularAutomaton.Core.Graphics.Effects;
using CellularAutomaton.Core.Services;
using CellularAutomaton.Core.Utilities;
using Guppy.Game.Common;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace CellularAutomaton.Core
{
    public sealed unsafe class World<TData> : IDisposable, IGuppyDrawable, IGuppyUpdateable
        where TData : unmanaged
    {
        private static readonly RasterizerState RasterizerState = new RasterizerState()
        {
            MultiSampleAntiAlias = true,
            SlopeScaleDepthBias = 0.5f
        };

        private readonly GridEffect _gridEffect;
        private readonly GraphicsDevice _graphics;
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private readonly ICellService<TData> _cellService;

        private bool _initialized;
        private Grid<TData> _grid;
        private VertexCellBuffer<TData> _gridVertices;
        private int* _cellUpdateOrder;
        private RenderTarget2D _renderTarget;

        public ref Grid<TData> Grid => ref _grid;

        public bool RenderAsleep
        {
            set
            {
                _gridEffect.RenderAsleep = value;
            }
        }

        public World(GridEffect gridEffect, GraphicsDevice graphics, Camera2D camera, SpriteBatch spriteBatch, ICellService<TData> cellService)
        {
            _graphics = graphics;
            _gridEffect = gridEffect;
            _camera = camera;
            _cellService = cellService;
            _spriteBatch = spriteBatch;

            _renderTarget = default!;
            _gridVertices = default!;

            _camera.Center = false;
        }

        public void Dispose()
        {
            if (_initialized == false)
            {
                return;
            }

            _grid.Dispose();
            _gridVertices?.Dispose();
            _renderTarget?.Dispose();


            Marshal.FreeHGlobal((nint)_cellUpdateOrder);

            _initialized = false;
        }

        public void Initialize(int width, int height, bool wrap, TData defaultData, Color defaultColor)
        {
            this.Dispose();

            _grid = new Grid<TData>(width, height, wrap, defaultData);
            _gridEffect.Width = width;

            _renderTarget = new RenderTarget2D(_graphics, width, height);
            _gridVertices = new VertexCellBuffer<TData>(_grid.Length, _graphics, defaultColor);
            _cellUpdateOrder = CalculateUpdateIndices(this.Grid.Length, width, height);

            _initialized = true;
        }

        public void Draw(GameTime gameTime)
        {
            _graphics.SetRenderTarget(_renderTarget);

            _camera.Update(gameTime);

            _gridEffect.View = _camera.View;
            _gridEffect.Projection = _camera.Projection;

            _graphics.BlendState = BlendState.AlphaBlend;
            _graphics.RasterizerState = RasterizerState;

            _gridVertices.Flush();

            foreach (EffectPass pass in _gridEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.DrawPrimitives(PrimitiveType.PointList, 0, _gridVertices.Length);
            }

            _graphics.SetRenderTarget(null);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, _graphics.Viewport.Bounds, Color.White);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Grid.Length; i++)
            {
                int index = _cellUpdateOrder[i];
                this.Update(ref this.Grid.Cells[index]);
            }

            for (int i = 0; i < this.Grid.Length; i++)
            {
                int index = _cellUpdateOrder[i];
                this.Grid.Cells[index].Reset();
            }
        }

        private void Update(ref Cell<TData> cell)
        {
            if (cell.Asleep)
            {
                return;
            }

            if (cell.Updated)
            {
                return;
            }

            _cellService.Update(ref cell, ref this.Grid, _gridVertices);
        }

        private static int* CalculateUpdateIndices(int length, int height, int width)
        {
            int* indices = (int*)Marshal.AllocHGlobal(length * sizeof(int));

            for (int i = 0; i < length; i++)
            {
                indices[i] = -1;
            }

            int index = 0;
            for (int y = height - 1; y >= 0; y--)
            {
                for (int i = 0; i < width; i++)
                {
                    int x = 0;
                    if (i % 2 == 0)
                    {
                        x = i;
                    }
                    else
                    {
                        x = (width - (width % 2)) - i;
                    }

                    indices[index++] = x + (y * width);
                }
            }

            for (int i = 0; i < length; i++)
            {
                index = indices[i];
                if (index >= length || index < 0)
                {
                    throw new Exception();
                }
            }

            return indices;
        }
    }
}
