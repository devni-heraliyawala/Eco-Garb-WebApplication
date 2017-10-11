using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    public class Edge
    {
        private int _cost;
        private Vector2D _pointA, _pointB;
        private static int EdgeIDSequencer = 0;
        private int _edgeID;


        #region Properties

        public int EdgeID
        {
            get
            {
                return _edgeID;
            }
        }

        public int Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }

        public Vector2D PointA
        {
            get
            {
                return _pointA;
            }
        }

        public Vector2D PointB
        {
            get
            {
                return _pointB;
            }
        }

        #endregion

        public Edge(Vector2D firstPoint, Vector2D secondPoint, int cost)
        {
            _cost = cost;
            _pointA = firstPoint;
            _pointB = secondPoint;
            _edgeID = ++EdgeIDSequencer;
        }

        public Vector2D GetTheOtherVertex(Vector2D baseVertex)
        {
            if (baseVertex == _pointA)
            {
                return _pointB;
            }
            else if (baseVertex == _pointB)
            {
                return _pointA;
            }
            else
            {
                // somehow the base vertex doesn't equal to either point A or B
                return null;
            }
        }

        public override string ToString()
        {
            return "Edge ID: " + _edgeID.ToString()
                + "; Connected to vertices " + _pointA.VertexID + " and "
                + _pointB.VertexID + " at a cost of " + _cost;
        }
    }
}