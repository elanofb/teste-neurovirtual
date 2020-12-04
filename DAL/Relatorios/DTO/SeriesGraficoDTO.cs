using System.Collections.Generic;

namespace DAL.Relatorios {

	//
	public class SeriersGraficoDTO {
        public string name { get; set; }
        public List<int> data { get; set; }    
        public string color { get; set; }    
        public DataLabelsDTO dataLabels  { get; set; }
        public int id { get; set; }
	}

    public class DataLabelsDTO {
        public bool enabled { get; set; }
        public string align { get; set; }
        public string backgroundColor { get; set; }
        public string color { get; set; }
        public string verticalAlign { get; set; }
        public int rotation { get; set; }
    }
}