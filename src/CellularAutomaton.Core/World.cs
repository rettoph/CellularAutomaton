﻿using CellularAutomaton.Core.Graphics.Effects;
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
        private VertexCellBuffer _gridVertices;
        private int* _cellUpdateOrder;
        private RenderTarget2D _renderTarget;

        public ref Grid<TData> Grid => ref _grid;

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
        }

        public void Initialize(int width, int height, bool wrap, TData defaultData, Color defaultColor)
        {
            this.Dispose();

            _grid = new Grid<TData>(width, height, wrap, defaultData);
            _gridEffect.Width = width;

            _renderTarget = new RenderTarget2D(_graphics, width, height);
            _gridVertices = new VertexCellBuffer(_grid.Length, _graphics, defaultColor);
            _cellUpdateOrder = CalculateUpdateIndices(this.Grid.Length);

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
                ref Cell<TData> cell = ref this.Grid.Cells[index];
                this.Update(ref cell);
            }
        }

        private void Update(ref Cell<TData> cell)
        {
            try
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
            finally
            {
                cell.Reset();
            }
        }

        private static int* CalculateUpdateIndices(int length)
        {
            int* indices = (int*)Marshal.AllocHGlobal(length * sizeof(int));

            int evenLength = (length + (length % 2)) / 2;
            for (int i = 0; i < evenLength; i++)
            {
                indices[i * 2] = i * 2;
            }

            int oddLength = length / 2;
            for (int i = 0; i < oddLength; i++)
            {
                indices[(i * 2) + 1] = (length - (length % 2)) - ((i * 2) + 1);
            }

            for (int i = 0; i < length; i++)
            {
                if (i >= length || i < 0)
                {
                    throw new Exception();
                }
            }

            return indices;
        }
    }
}
