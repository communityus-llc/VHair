﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace VHair
{
    /// <summary>
    /// The data saved in the asset should _NEVER_ be changed at runtime, but instead be used as a source from where to create a copy (instance).
    /// </summary>
    [CreateAssetMenu(fileName = "HairAsset", menuName = "VHair/Hair Asset")]
    public class HairAsset : ScriptableObject
    {
        /// <summary>
        /// The vertices used for the hair.
        /// </summary>
        [SerializeField]
        private Vector3[] vertices;

        /// <summary>
        /// The strands of this hair asset.
        /// </summary>
        [SerializeField]
        private HairStrand[] strands;

        /// <summary>
        /// The movability bits of the hair.
        /// Bit index maps to <see cref="vertexCount"/>, <see cref="HairMovability"/> for the utilities to access this data.
        /// </summary>
        [SerializeField]
        private uint[] movability;

        /// <summary>
        /// Whether or not this asset already has data imported.
        /// </summary>
        public bool wasImported = false;

        public int strandCount
        {
            get { return this.strands.Length; }
        }

        public int vertexCount
        {
            get { return this.vertices.Length; }
        }

        public void GetVertexData(out Vector3[] vertices)
        {
            vertices = new Vector3[this.vertices.Length];
            System.Array.Copy(this.vertices, vertices, vertices.Length);
        }

        public void GetStrandData(out HairStrand[] strands)
        {
            strands = new HairStrand[this.strands.Length];
            System.Array.Copy(this.strands, strands, strands.Length);
        }

        /// <summary>
        /// Creates a copy of <see cref="movability"/>
        /// </summary>
        /// <param name="movability"></param>
        public void GetMovabilityData(out uint[] movability)
        {
            movability = new uint[Mathf.CeilToInt(this.vertices.Length / 32f)];
            System.Array.Copy(this.movability, movability, movability.Length);
        }

        public void GetRawDataCopy(out Vector3[] vertices, out HairStrand[] strands, out uint[] movability)
        {
            GetVertexData(out vertices);
            GetStrandData(out strands);
            GetMovabilityData(out movability);
        }

        /// <summary>
        /// Initializes (reallocates and overwrites internal-) vertex data.
        /// </summary>
        /// <param name="vertices">The vertex data to write to the asset.</param>
        public void InitializeVertices(Vector3[] vertices)
        {
            this.vertices = new Vector3[vertices.Length];
            vertices.CopyTo(this.vertices, 0);
        }

        /// <summary>
        /// Initializes (reallocates and overwrites internal- ) strand data.
        /// </summary>
        /// <param name="strands">The strand data to write to the asset.</param>
        public void InitializeStrands(HairStrand[] strands)
        {
            this.strands = new HairStrand[strands.Length];
            strands.CopyTo(this.strands, 0);
        }

        /// <summary>
        /// Initializes (reallocates and overwrites internal- ) movability data.
        /// </summary>
        /// <param name="movability">The mobility data to write, if null every vertex is marked as immovable.</param>
        public void InitializeMovability(uint[] movability = null)
        {
            this.movability = HairMovability.CreateData(vertices.Length);
            
            if (movability != null)
            {
                if (movability.Length != this.movability.Length)
                    throw new System.ArgumentException("Incorrect movability array size, length should be ceil(vertexCount / 32f)");
                movability.CopyTo(this.movability, 0);
            }
        }
    }
}