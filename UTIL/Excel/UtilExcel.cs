using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelGenerator.SpreadSheet;
using System.IO;
using System.Web.UI;

namespace UTIL.Excel {
    public class UtilExcel {

        public UtilWorkbook _UtilWorkbook { get; set; }
        public Worksheet _CurrentWorksheet { get; set; }
        private string[] _selectedCols { get; set; }

        //
        public UtilExcel() {
            this._UtilWorkbook = new UtilWorkbook();
        }


		/**
		 * Criar uma nova planilha com o nome
		 */
		public void setNovaPlanilha(string tituloPlanilha) { 
			this._CurrentWorksheet = new Worksheet(tituloPlanilha);
		}

		/**
		 * Adicioanr a planilha atual ao workbook e Capturar os bytes do documento gerado
		 */ 
		public byte[] getBytesPlanilha() { 
			 this._UtilWorkbook.Worksheets.Add( this._CurrentWorksheet );
            return this._UtilWorkbook.getBytes();
		}

        /**
		 * Configurar os nomes das colunas para a planilha atual.
		 */
        public void adicionarColunas(string[] columnsName) {
            if(columnsName != null && columnsName.Length > 0) {
                Row NewRow = new Row();

                foreach(string columnName in columnsName) { 
                    NewRow.Cells.Add(new Cell(columnName));
                }

                this._CurrentWorksheet.Rows.Add(NewRow);
            }
        }

		/**
		 * 
		 */
		public void adicionarLinha(Row NovaLinha) { 
			this._CurrentWorksheet.Rows.Add(NovaLinha);
		}


        //
        private void loadItens<T>(IList<T> listItens) {
            if(this._selectedCols == null || this._selectedCols.Length < 1) {
                var properties = typeof(T).GetProperties();
                this._selectedCols = properties.Select(x => x.Name).ToArray();                
            }

            foreach(T Item in listItens) {
                Row NewRow = new Row();
                foreach(string colName in this._selectedCols) {
                    var propInfo = typeof(T).GetProperty(colName);
                    var value = propInfo.GetValue(Item, null);
                    string cellValue = (value == null? "-": value.ToString());

                    NewRow.Cells.Add(new Cell(cellValue));
                }

                this._CurrentWorksheet.Rows.Add(NewRow);
            }
        }


        //
        public byte[] listToBytes<T>(IList<T> listItens, string worksheetName, string[] selectedCols, string[] columnsName) {
            this._CurrentWorksheet = new Worksheet(worksheetName);
            this._selectedCols = selectedCols;

            if(columnsName != null && columnsName.Length > 0) { 
                this.adicionarColunas(columnsName);
            }

            this.loadItens<T>(listItens);

            this._UtilWorkbook.Worksheets.Add( this._CurrentWorksheet );

            return this._UtilWorkbook.getBytes();
        }

		/**
		 * 
		 */
		public void downloadExcel(System.Web.HttpResponseBase OResponse, System.Web.UI.WebControls.GridView OGrid, string nomeDownload) { 
			OResponse.ClearContent();
			OResponse.Buffer = true;
			OResponse.AddHeader("content-disposition", "attachment; filename="+nomeDownload);
			OResponse.ContentType = "application/ms-excel";
            OResponse.ContentEncoding = Encoding.GetEncoding(28592);

			OResponse.Charset = "";
			StringWriter sw = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(sw);

			OGrid.RenderControl(htw);

			OResponse.Output.Write(sw.ToString());
			OResponse.Flush();
			OResponse.End();
		}
	    
	    //
	    public static string columnIndexToLetter(int indice){
            
		    int div = indice;
		    string colLetter = String.Empty;
		    int mod = 0;
            
		    while (div > 0){
                
			    mod = (div - 1) % 26;
			    colLetter = (char)(65 + mod) + colLetter;
			    div = (int)((div - mod) / 26);
                
		    }
            
		    return colLetter;
            
	    } 


    }
}
