using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    public class Vector2D
    {
        public const int INFINITY = int.MaxValue;
        private String _lat, _lng;
        private bool _deadend;
        private string _address;
        private string _cname;
        private string _branch;
        private double _qty;
        private bool _visited;
        private static int VertexIDSequencer = 0;
        private int _vertexID;
        private int _aggregateCost;
        private Edge _edgeWithLowestCost;


        #region Properties

        public int VertexID
        {
            get

            {
                return _vertexID;
            }
        }

        public int AggregateCost
        {
            get
            {
                return _aggregateCost;
            }
            set
            {
                _aggregateCost = value;
            }
        }

        public String Lat
        {
            get
            {
                return _lat;
            }
            set
            {
                _lat = value;
            }
        }

        public String  Lng
        {
            get
            {
                return _lng;
            }
            set
            {
                _lng = value;
            }
        }

        public bool Deadend
        {
            get
            {
                return _deadend;
            }
        }

        public bool Visited
        {
            get
            {
                return _visited;
            }
            set
            {
                _visited = value;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return _cname;
            }
            set
            {
                _cname = value;
            }
        }

        public string Branch
        {
            get
            {
                return _branch;
            }
            set
            {
                _branch = value;
            }
        }

        public double TrashQuantity
        {
            get
            {
                return _qty;
            }
            set
            {
                _qty = value;
            }
        }


        // Internal members
        internal Edge EdgeWithLowestCost
        {
            get
            {
                return _edgeWithLowestCost;
            }
            set
            {
                _edgeWithLowestCost = value;
            }
        }

        #endregion 

        public Vector2D(String lat, String lng, bool deadend, string address, string cname, string branch, string qty)
        {
            _visited = false;
            _lat = lat;
            _lng = lng;
            _deadend = deadend;
            _address = address;
            _cname = cname;
            _branch = branch;
            _qty = Convert.ToDouble(qty);
            _aggregateCost = INFINITY;
            _vertexID = ++VertexIDSequencer;
            _edgeWithLowestCost = null;
        }

        public override string ToString()
        {
            return "Vertex ID: " + _vertexID; // + "; Coords: X=" + lat + ", Y=" + _lng;
        }
    }
}