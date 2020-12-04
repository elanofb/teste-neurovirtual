using System.Collections.Generic;
using System.IO;

namespace System {

    public static class UtilIO {


        //Capturar a extensao de um arquivo com base em seu path absoluto
        public static string getExtension(string fileName) {
            return fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
        }

        //Criar um diretorio e seus subdiretorios que nao existirem
        public static string createFolder(string path) {

            DirectoryInfo Dir = new DirectoryInfo(path);

            if (!Dir.Exists) {
                Dir.Create();
            }

            return path;
        }

        //
        public static FileInfo createFile(string pathFolder, string fileName, bool combineDefaultFolder = true) {
            
            string fullPathFolder;
            
            if (combineDefaultFolder) {
                fullPathFolder = Path.Combine(UtilConfig.pathAbsUploadFiles, pathFolder);
            } else {
                fullPathFolder = pathFolder;
            }

            createFolder(fullPathFolder);

            FileInfo F = new FileInfo(Path.Combine(fullPathFolder, fileName));
            
            FileStream Fs = F.Create();
            
            Fs.Close();
            
            return F;
        }

        //Escrever em um arquivo
        //Se nao existir o arquivo, o sistema o criará
        public static void writeFile(string pathFile, string content) {

            if (!File.Exists(pathFile)) {
                FileInfo F = new FileInfo(pathFile);
                createFolder(F.DirectoryName);
                File.Create(pathFile).Close();
            }

            using (TextWriter Writer = File.AppendText(pathFile)) {

                Writer.Write(content);

                Writer.Close();
            }

        }

        //Salvar o strem em um arquivo
        public static FileInfo saveStreamToFile(string fileFullPath, Stream stream) {
            if (stream.Length == 0) {
                FileInfo F = new FileInfo(fileFullPath);
                F.Create().Close();
                return F;
            }

            FileInfo newFile = null;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = File.Create(fileFullPath, (int)stream.Length)) {
                // Fill the bytes[] array with the stream data
                var bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);

                newFile = new FileInfo(fileFullPath);
            }

            return newFile;
        }

        //Salvar o strem em um arquivo
        public static FileInfo saveBytesToFile(string filePath, byte[] bytesFile, FileMode fileMode = FileMode.Append) {
            
            using (var stream = new FileStream(filePath, fileMode)) {

                stream.Write(bytesFile, 0, bytesFile.Length);

            }

            return new FileInfo(filePath);
        }

        //
        public static List<FileInfo> listDirectoryFiles(string path, string filtro = "*.txt") {
            // Verifica se o Diretório Existe
            if (!Directory.Exists(path)) return null;

            // Array de string que irá receber todos os nomes dos arquivos encontrados
            string[] strArquivos;
            if (filtro.Trim() == "")
                strArquivos = Directory.GetFiles(path);
            else
                strArquivos = Directory.GetFiles(path, filtro);

            // Verifica se algum arquivo foi encontrado
            if (strArquivos.Length == 0) return null;

            // Cria uma lista de FileInfo e preenche com informações do Array
            List<FileInfo> arquivos = new List<FileInfo>();
            for (int i = 0; i < strArquivos.Length; i++) {
                arquivos.Add(new FileInfo(strArquivos[i]));
            }

            return arquivos;
        }

        public static string copiarArquivo(FileInfo OFile, string pathNovoDestino, string nomeArquivo, bool renomearSeExistir = true) {
            if (!OFile.Exists) {
                return "";
            }

            string nomeArquivoCompleto = Path.Combine(pathNovoDestino, nomeArquivo);

            if (renomearSeExistir) {
                int cont = 1;
                while (File.Exists(nomeArquivoCompleto)) {
                    string prefixo = String.Concat("(", cont.ToString().PadLeft(3, '0'), ")");

                    if (nomeArquivo.Contains(prefixo)) {
                        nomeArquivo = nomeArquivo.Replace(prefixo, String.Concat("(", (++cont).ToString().PadLeft(3, '0'), ")"));
                        nomeArquivoCompleto = Path.Combine(pathNovoDestino, nomeArquivo);
                    } else {
                        nomeArquivoCompleto = Path.Combine(pathNovoDestino, String.Concat(prefixo, nomeArquivo));
                        cont++;
                    }
                }
            } else {
                if (File.Exists(nomeArquivoCompleto)) {
                    File.Delete(nomeArquivoCompleto);
                }
            }

            OFile.CopyTo(nomeArquivoCompleto);

            return nomeArquivoCompleto;
        }

        //Capturar os bytes de um arquivo
        public static byte[] getBytes(string caminho) {
            return File.ReadAllBytes(caminho);
        }

    }
}
