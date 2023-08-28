using System.Drawing;

Console.WriteLine("Inicando redimensionador...");

//Com esse código nos passamos um thread do processador para execultar o método Redimensionar
Thread thread = new Thread(Redimensionar);
thread.Start();

Console.ReadKey();

static void Redimensionar()
{
    #region "Diretorios"
    //Region serve para poder separa uma parte do seu código pra poder fechalo
    string diretorio_entrada = "Arquivos_Entrada";
    string diretorio_redimensionado = "Arquivos_Redimensionados";
    string diretorio_finalizados = "Arquivos_Finalizados";

    if (!Directory.Exists(diretorio_entrada)) //Vai verificar se tem esse arquivo no diretorio da compilação
    {
        Directory.CreateDirectory(diretorio_entrada);

    }
    if (!Directory.Exists(diretorio_redimensionado))
    {
        Directory.CreateDirectory(diretorio_redimensionado);

    }
    if (!Directory.Exists(diretorio_finalizados))
    {
        Directory.CreateDirectory(diretorio_finalizados);
    }
    #endregion

    FileStream fileStream;
    FileInfo fileInfo;

    while (true)
    {
        //Vai ficar olhando para a pasta de entrada
        //SE tiver arquivo ele vai redimensionar
        var arquivos_entrada = Directory.EnumerateFiles(diretorio_entrada);

        //Ler o tamanho que ira redimensionar
        int novaAltura = 200;

        foreach (var arquivo in arquivos_entrada)
        {
            fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            fileInfo = new FileInfo(arquivo);

            string caminho = Environment.CurrentDirectory + @"\"
                + diretorio_redimensionado + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name; 

            //Redimensionando
            Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);
            
            //Fechar o arquivo
            fileStream.Close();

            //Move o arquivos de entrada para a pasta de finalizados 
            string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizados + @"\" + fileInfo.Name;
            fileInfo.MoveTo(caminhoFinalizado);

        }
        Thread.Sleep(new TimeSpan(0,0,5));
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="imagem">Imagem a ser redimensionado</param>
/// <param name="altura">Altura que desejamos redimensionar</param>
/// <param name="caminho">Caminho para onde vai a imagem redimensionada</param>
/// <returns></returns>
static void Redimensionador(Image imagem, int altura, string caminho)
{
    double ration = (double)altura / imagem.Height;
    int novaLargura = (int)(imagem.Width * ration);
    int novaAltura = (int)(imagem.Height * ration);

    Bitmap novaImage = new Bitmap(novaLargura, novaAltura);
    using(Graphics g = Graphics.FromImage(novaImage))
    {
        g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
    }

    //Copia os arquivos redimensionados para a pasta de redimensionandos
    novaImage.Save(caminho);
    imagem.Dispose();
}