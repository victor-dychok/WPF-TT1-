using Filework_MVVM_B1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Filework_MVVM_B1.ViewModels
{
    internal class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const int _NumberOfFiles = 100;
        private DBContext _dbContext = new DBContext();
        private static DirectoryInfo dirInfo = new DirectoryInfo(@"D:\учёба\4 курс\больше кода\Filework_MVVM_B1\");

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int _NumberOfCreatedFiles;
        public int NumberOfCreatedFiles
        {
            get { return _NumberOfCreatedFiles; }
            set
            {
                _NumberOfCreatedFiles = value;
                OnPropertyChanged(nameof(NumberOfCreatedFiles));
            }
        }

        private int _NumberOfJoinedFiles;
        public int NumberOfJoinedFiles
        {
            get { return _NumberOfJoinedFiles; }
            set
            {
                _NumberOfJoinedFiles = value;
                OnPropertyChanged(nameof(NumberOfJoinedFiles));
            }
        }

        private int _NumberOfDeletedStrings;
        public int NumberOfDeletedStrings
        {
            get { return _NumberOfDeletedStrings; }
            set
            {
                _NumberOfDeletedStrings = value;
                OnPropertyChanged(nameof(NumberOfDeletedStrings));
            }
        }

        private string _TextToDelete;

        public string TextToDelete
        {
            get { return _TextToDelete; }
            set
            {
                _TextToDelete = value;
                OnPropertyChanged(nameof(TextToDelete));
            }
        }

        private int _ExportedFiles;

        public int ExportedFiles
        {
            get { return _ExportedFiles; }
            set 
            { 
                _ExportedFiles = value;
                OnPropertyChanged(nameof(ExportedFiles));
            }
        }

        private int _ExportedStrings;

        public int ExportedStrings
        {
            get { return _ExportedStrings; }
            set 
            { 
                _ExportedStrings = value;
                OnPropertyChanged(nameof(ExportedStrings));
            }
        }

        private int _NumberOfDeletedStrings_DBExport;

        public int NumberOfDeletedStrings_DBExport
        {
            get { return _NumberOfDeletedStrings_DBExport; }
            set 
            { 
                _NumberOfDeletedStrings_DBExport = value; 
                OnPropertyChanged(nameof(NumberOfDeletedStrings_DBExport)); 
            }
        }

        private string _ExportingToDatabaseStatus;

        public string ExportingToDatabaseStatus
        {
            get { return _ExportingToDatabaseStatus; }
            set 
            { 
                _ExportingToDatabaseStatus = value;
                OnPropertyChanged(nameof(ExportingToDatabaseStatus));
            }
        }





        public MainVM()
        {
            NumberOfCreatedFiles = 0;
            NumberOfJoinedFiles = 0;
        }

        public ICommand CreateFiles
        {
            get
            {
                return new DelegateComand((obj) =>
                Task.Factory.StartNew(() =>
                {
                    TextGenerator generator = new TextGenerator();


                    for (int j = 0; j < _NumberOfFiles; ++j)
                    {
                        var list = generator.GetStringsRange(100000); //geting 100 000 strings for current file

                        string fileName = $@"D:\учёба\4 курс\больше кода\Filework_MVVM_B1\{j}.txt"; //current directory

                        try
                        {
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }

                            // Create a new file 
                            using (FileStream fs = File.Create(fileName))
                            {
                                // Adding the strings to file
                                int count = list.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    Byte[] currentText = new UTF8Encoding(true).GetBytes(list[i]);
                                    fs.Write(currentText, 0, currentText.Length);
                                }
                            }

                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.ToString());
                        }
                        ++NumberOfCreatedFiles;
                    }
                })
                );
            }
        }

        public ICommand JoinFiles
        {
            get
            {
                return new DelegateComand(obj => Task.Factory.StartNew(() =>
                {
                    var enFils = dirInfo.GetFiles().Where(i => i.Extension == ".txt").ToList();
                    NumberOfDeletedStrings = 0;
                    NumberOfJoinedFiles = 0;

                    try
                    {
                        string fileName = @"D:\учёба\4 курс\больше кода\Filework_MVVM_B1\200.txt";

                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }

                        using (FileStream fs = File.Create(fileName))
                        {
                            int len = enFils.Count;
                            for (int i = 0; i < len; i++)
                            {

                                string buff;
                                string[] strings;
                                using (StreamReader sr = new StreamReader(enFils[i].FullName))
                                {
                                    buff = sr.ReadToEnd();
                                }

                                strings = buff.Split('\n');

                                int strSize = strings.Length;


                                for (int l = 0; l < strSize; l++)
                                {
                                    if (_TextToDelete != null && strings[l].Contains(_TextToDelete))
                                    {
                                        ++NumberOfDeletedStrings;
                                        strings[l] = string.Empty;
                                    }
                                    else
                                    {
                                        strings[l] += "\n";
                                    }
                                }

                                foreach (var str in strings)
                                {
                                    Byte[] currentText = new UTF8Encoding(true).GetBytes(str);
                                    fs.Write(currentText, 0, currentText.Length);
                                }
                                NumberOfJoinedFiles++;
                            }
                        }

                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex.ToString());
                    }
                }));
            }
        }

        public ICommand ExportFiles
        {
            get 
            {
                return new DelegateComand(obj => Task.Factory.StartNew(() =>
                {
                    var enFils = dirInfo.GetFiles().Where(i => i.Extension == ".txt").ToList();
                    List<StringModel> lines = new List<StringModel>();

                    int len = enFils.Count;
                    
                    for (int i = 0; i < len; i++)
                    {

                        string buff;
                        string[] strings;
                        using (StreamReader sr = new StreamReader(enFils[i].FullName))
                        {
                            buff = sr.ReadToEnd();
                        }

                        strings = buff.Split('\n');

                        int strSize = strings.Length;


                        for (int l = 0; l < strSize - 1; l++)
                        {
                            if (_TextToDelete != null && strings[l].Contains(_TextToDelete))
                            {
                                ++NumberOfDeletedStrings_DBExport;
                                strings[l] = string.Empty;
                            }
                            else
                            {
                                _dbContext.Lines.Add(ParceToModel(strings[l]));
                                ++ExportedStrings;
                            }
                        }
                        ++ExportedFiles;
                        ExportingToDatabaseStatus = "Saving local changes to database";
                        _dbContext.SaveChanges();
                        ExportingToDatabaseStatus = "Preparing...";
                    }
                
                    MessageBox.Show(lines.Count.ToString());
                }));
            }
        }

        private StringModel ParceToModel(string str)
        {
            if(str != "")
            {
                var s = str.Split("||");
                return new StringModel
                {
                    LatinLetters = s[1],
                    KirilicLetters = s[2],
                    IntegerNumber = Convert.ToInt32(s[3]),
                    RealNumber = Convert.ToDouble(s[4]),
                };
            }
            else
            {
                return null;
            }
        }
    }
}
