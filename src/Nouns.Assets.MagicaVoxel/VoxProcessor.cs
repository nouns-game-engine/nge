using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NGE.Graphics;

namespace Nouns.Assets.MagicaVoxel
{
    public static class VoxProcessor
    {
        public static VoxelModel ToModel(this VoxFile voxFile)
        {
            var data = new VoxelModel();

            if (voxFile.Main?.Children == null)
                throw new InvalidOperationException("invalid VOX file");

            var voxels = new List<Voxel>();
            var colors = new List<uint>();
            var size = Vector3.Zero;

            foreach (var chunk in voxFile.Main.Children)
            {
                switch (chunk)
                {
                    case Model {Voxels: { }} m:
                    {
                        foreach (var v in m.Voxels)
                            voxels.Add(v);

                        if (m.Size != null)
                            size = new Vector3(m.Size.X, m.Size.Y, m.Size.Z);

                        break;
                    }
                    case Palette {Colors: { }} p:
                    {
                        foreach (var color in p.Colors)
                        {
                            colors.Add(color.PackedValue);
                        }
                        break;
                    }
                }
            }

            if (colors.Count == 0)
                colors.AddRange(VoxFile.defaultPalette);

            CalculateSharedFaces(voxels);
            voxels = CullHiddenFaces(voxels);

            var worldSize = Vector3.One;
            var scale = worldSize / size;
            var offset = new Vector3(1f, 1f, 1f) * (worldSize / -2f);

            var c000 = new Point3(0, 0, 0);
            var c100 = new Point3(1, 0, 0);
            var c010 = new Point3(0, 1, 0);
            var c110 = new Point3(1, 1, 0);
            var c001 = new Point3(0, 0, 1);
            var c101 = new Point3(1, 0, 1);
            var c011 = new Point3(0, 1, 1);
            var c111 = new Point3(1, 1, 1);

            foreach (var voxel in voxels)
            {
                var p000 = voxel.AsPoint.Add(c000);
                var p100 = voxel.AsPoint.Add(c100);
                var p010 = voxel.AsPoint.Add(c010);
                var p110 = voxel.AsPoint.Add(c110);
                var p001 = voxel.AsPoint.Add(c001);
                var p101 = voxel.AsPoint.Add(c101);
                var p011 = voxel.AsPoint.Add(c011);
                var p111 = voxel.AsPoint.Add(c111);

                var p0 = p000.ToVector3();
                var p1 = p100.ToVector3();
                var p2 = p010.ToVector3();
                var p3 = p110.ToVector3();
                var p4 = p001.ToVector3();
                var p5 = p101.ToVector3();
                var p6 = p011.ToVector3();
                var p7 = p111.ToVector3();

                Vector3.Multiply(ref p0, ref scale, out p0);
                Vector3.Add(ref p0, ref offset, out p0);

                Vector3.Multiply(ref p1, ref scale, out p1);
                Vector3.Add(ref p1, ref offset, out p1);

                Vector3.Multiply(ref p2, ref scale, out p2);
                Vector3.Add(ref p2, ref offset, out p2);

                Vector3.Multiply(ref p3, ref scale, out p3);
                Vector3.Add(ref p3, ref offset, out p3);

                Vector3.Multiply(ref p4, ref scale, out p4);
                Vector3.Add(ref p4, ref offset, out p4);

                Vector3.Multiply(ref p5, ref scale, out p5);
                Vector3.Add(ref p5, ref offset, out p5);

                Vector3.Multiply(ref p6, ref scale, out p6);
                Vector3.Add(ref p6, ref offset, out p6);

                Vector3.Multiply(ref p7, ref scale, out p7);
                Vector3.Add(ref p7, ref offset, out p7);

                data.vertex.color.PackedValue = colors[voxel.I];

                if ((voxel.SharedFaces & Faces.Front) == 0)
                {
                    data.vertex.normal = Vector3.Forward;

                    AddVertex(data, p1);
                    AddVertex(data, p3);
                    AddVertex(data, p0);
                    AddVertex(data, p0);
                    AddVertex(data, p3);
                    AddVertex(data, p2);
                }

                if ((voxel.SharedFaces & Faces.Back) == 0)
                {
                    data.vertex.normal = Vector3.Backward;

                    AddVertex(data, p4);
                    AddVertex(data, p6);
                    AddVertex(data, p5);
                    AddVertex(data, p5);
                    AddVertex(data, p6);
                    AddVertex(data, p7);
                }

                if ((voxel.SharedFaces & Faces.Left) == 0)
                {
                    data.vertex.normal = Vector3.Left;

                    AddVertex(data, p2);
                    AddVertex(data, p6);
                    AddVertex(data, p0);
                    AddVertex(data, p0);
                    AddVertex(data, p6);
                    AddVertex(data, p4);
                }

                if ((voxel.SharedFaces & Faces.Right) == 0)
                {
                    data.vertex.normal = Vector3.Right;

                    AddVertex(data, p1);
                    AddVertex(data, p5);
                    AddVertex(data, p3);
                    AddVertex(data, p3);
                    AddVertex(data, p5);
                    AddVertex(data, p7);
                }

                if ((voxel.SharedFaces & Faces.Top) == 0)
                {
                    data.vertex.normal = Vector3.Up;

                    AddVertex(data, p7);
                    AddVertex(data, p6);
                    AddVertex(data, p3);
                    AddVertex(data, p3);
                    AddVertex(data, p6);
                    AddVertex(data, p2);
                }

                if ((voxel.SharedFaces & Faces.Bottom) == 0)
                {
                    data.vertex.normal = Vector3.Down;

                    AddVertex(data, p5);
                    AddVertex(data, p1);
                    AddVertex(data, p4);
                    AddVertex(data, p4);
                    AddVertex(data, p1);
                    AddVertex(data, p0);
                }
            }

            return data;
        }

        private static void AddVertex(VoxelModel data, Vector3 position)
        {
            data.vertex.position = position;
            if (!data.vertexMap.TryGetValue(data.vertex, out var index))
            {
                index = data.vertexBuffer.Count;
                data.vertexBuffer.Add(data.vertex);
                data.vertexMap.Add(data.vertex, index);
            }
            data.indexBuffer.Add(index);
        }

        private static void CalculateSharedFaces(this IReadOnlyList<Voxel> voxels)
        {
            var blocks = new HashSet<Point3>();

            for (var i = 0; i < voxels.Count; i++)
            {
                var block = new Point3(voxels[i].X, voxels[i].Y, voxels[i].Z);
                blocks.Add(block);
            }

            for (var i = 0; i < voxels.Count; i++)
            {
                var block = new Point3(voxels[i].X, voxels[i].Y, voxels[i].Z);

                if (blocks.Contains(new Point3(block.x - 1, block.y, block.z)))
                    voxels[i].SharedFaces |= Faces.Left;

                if (blocks.Contains(new Point3(block.x + 1, block.y, block.z)))
                    voxels[i].SharedFaces |= Faces.Right;

                if (blocks.Contains(new Point3(block.x, block.y - 1, block.z)))
                    voxels[i].SharedFaces |= Faces.Bottom;

                if (blocks.Contains(new Point3(block.x, block.y + 1, block.z)))
                    voxels[i].SharedFaces |= Faces.Top;

                if (blocks.Contains(new Point3(block.x, block.y, block.z + 1)))
                    voxels[i].SharedFaces |= Faces.Back;

                if (blocks.Contains(new Point3(block.x, block.y, block.z - 1)))
                    voxels[i].SharedFaces |= Faces.Front;
            }
        }

        private static List<Voxel> CullHiddenFaces(this IReadOnlyList<Voxel> voxels)
        {
            var visible = new List<Voxel>();

            for (var i = 0; i < voxels.Count; i++)
            {
                var voxel = voxels[i];
                if (voxel.SharedFaces == Faces.All)
                    continue;
                visible.Add(voxel);
            }

            return visible;
        }
    }
}