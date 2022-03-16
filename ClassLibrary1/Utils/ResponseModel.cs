using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Utils
{
    public class ResponseModel<T>
    {
        public bool HasNext { get; set; } 
        public bool HasPrevious { get; set; }
        public string NextUrl { get; set; }
        public string PreviousUrl { get; set; }
        public string JsonString { get; set; }
        public T JsonData { get; set; }
        public List<T> JsonDataList { get; set; }
        public ResponseModel()
        {
            HasNext = false;
            HasPrevious = false;
            NextUrl = "";
            PreviousUrl = "";
            JsonDataList = new List<T>();
        }

        public void UpdateModelState(ResponseModel<T> responseModel)
        {
            this.HasNext = responseModel.HasNext;
            this.HasPrevious = responseModel.HasPrevious;
            this.NextUrl = responseModel.NextUrl;
            this.PreviousUrl = responseModel.PreviousUrl;
        }
    }
}
