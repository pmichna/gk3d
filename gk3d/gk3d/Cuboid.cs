using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace gk3d
{
    class Cuboid
    {
        public VertexPositionColorNormal[] Vertices { get; private set; }
        public int[] Indices { get; private set; }
        public Vector3 Center { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }
        private Color Color { get; set; }

        public Cuboid(Vector3 center, int width, int height, int depth, bool isVisibleInside, Color color)
        {
            Center = center;
            Color = color;
            Width = width;
            Height = height;
            Depth = depth;
            SetUpVertices();
            if (isVisibleInside)
                SetUpInnerIndices();
            else
                SetUpOuterIndices();
            CalculateNormalsForTriangleList();
        }

        private void SetUpVertices()
        {
            Vertices = new VertexPositionColorNormal[8];
            Vertices[0].Position = new Vector3(Center.X - Width / 2f, Center.Y - Height / 2f, Center.Z + Depth / 2f);
            Vertices[0].Color = Color;
            Vertices[1].Position = new Vector3(Center.X + Width / 2f, Center.Y - Height / 2f, Center.Z + Depth / 2f);
            Vertices[1].Color = Color;
            Vertices[2].Position = new Vector3(Center.X + Width / 2f, Center.Y - Height / 2f, Center.Z - Depth / 2f);
            Vertices[2].Color = Color;
            Vertices[3].Position = new Vector3(Center.X - Width / 2f, Center.Y - Height / 2f, Center.Z - Depth / 2f);
            Vertices[3].Color = Color;

            Vertices[4].Position = new Vector3(Center.X - Width / 2f, Center.Y + Height / 2f, Center.Z + Depth / 2f);
            Vertices[4].Color = Color;
            Vertices[5].Position = new Vector3(Center.X + Width / 2f, Center.Y + Height / 2f, Center.Z + Depth / 2f);
            Vertices[5].Color = Color;
            Vertices[6].Position = new Vector3(Center.X + Width / 2f, Center.Y + Height / 2f, Center.Z - Depth / 2f);
            Vertices[6].Color = Color;
            Vertices[7].Position = new Vector3(Center.X - Width / 2f, Center.Y + Height / 2f, Center.Z - Depth / 2f);
            Vertices[7].Color = Color;
        }

        private void SetUpOuterIndices()
        {
            Indices = new int[36];
            //bottom
            var side = new[] { 0, 1, 2, 3, 0 };
            var firstIndex = CreateSideTriangles(side, 0);
            //top
            side = new[] { 4, 7, 6, 5, 4 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //left
            side = new[] { 3, 7, 4, 0, 3 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //right
            side = new[] { 1, 5, 6, 2, 1 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //front
            side = new[] { 0, 4, 5, 1, 0 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //back
            side = new[] { 2, 6, 7, 3, 2 };
            CreateSideTriangles(side, firstIndex);
        }

        private void SetUpInnerIndices()
        {
            Indices = new int[36];
            //bottom
            var side = new[] { 0, 3, 2, 1, 0 };
            var firstIndex = CreateSideTriangles(side, 0);
            //top
            side = new[] { 4, 5, 6, 7, 4 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //left
            side = new[] { 3, 0, 4, 7, 3 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //right
            side = new[] { 1, 2, 6, 5, 1 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //front
            side = new[] { 0, 1, 5, 4, 0 };
            firstIndex = CreateSideTriangles(side, firstIndex);
            //back
            side = new[] { 2, 3, 7, 6, 2 };
            CreateSideTriangles(side, firstIndex);
        }

        private int CreateSideTriangles(IList<int> side, int firstIndex)
        {
            var sideIndex = 0;
            for (var i = 0; i < 2; i++)
            {
                Indices[firstIndex++] = side[sideIndex++];
                Indices[firstIndex++] = side[sideIndex++];
                Indices[firstIndex++] = side[sideIndex];
            }
            return firstIndex;
        }

        private void CalculateNormalsForTriangleList()
        {
            for (var i = 0; i < Vertices.Length; i++)
                Vertices[i].Normal = new Vector3(0, 0, 0);

            for (var i = 0; i < Indices.Length / 3; i++)
            {
                var firstVector = Vertices[Indices[i*3 + 1]].Position -
                                  Vertices[Indices[i*3]].Position;
                var secondVector = Vertices[Indices[i * 3 + 2]].Position -
                                  Vertices[Indices[i * 3]].Position;
                var normal = Vector3.Cross(secondVector, firstVector);
                normal.Normalize();

                Vertices[Indices[i*3]].Normal += normal;
                Vertices[Indices[i*3 + 1]].Normal += normal;
                Vertices[Indices[i*3 + 2]].Normal += normal;
            }

            for (var i = 0; i < Vertices.Length; i++)
                Vertices[i].Normal.Normalize();
        }
    }
}