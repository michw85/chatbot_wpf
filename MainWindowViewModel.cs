using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

using MyViber.Base;
using MyViber.ChatContacts;
using MyViber.ChatLog;
using MyViber.ChatBot;

namespace MyViber
{
    class MainWindowViewModel : Base.ViewModel
    {

        public ObservableCollection<ChatContactsNode> Contacts { get; set; }
        public ObservableCollection<ChatLogNode> Chat { get; set; }
        public string align { get; set; }
        string _nikName;
        public string userMsg { get; set; }

        public ChatContactsNode nikName
        {
            set
            {
                _nikName = value.nikName;
                if (value != null)
                {
                    MessageBox.Show(_nikName);
                    Chat = ChatLogModel.getInstance().getChat(_nikName);
                    OnPropertyChanged(nameof(Chat));
                }
            }
        }



       
        public Command SendMsgToChat { get; set; }

        private void setupCommands()
        {
          
            SendMsgToChat = new Command(() =>
            {
                if (this._nikName == null)
                {
                    MessageBox.Show("Select User to send " + userMsg);
                    return;
                }
               
                ChatLogModel.getInstance().addMsgToChat(_nikName, "You", userMsg);

 

                string[] words = userMsg.Split();
                foreach (string word in words)
                {
                   
                    ObservableCollection<string> Answers = ChatBot.ChatBotModel.getInstance().getAnswers(word);
                    if (Answers.Count < 1)
                    {
                        ChatLogModel.getInstance().addMsgToChat(_nikName, _nikName, "Ура!!!");
                    }
                    else
                    {
                        ChatLogModel.getInstance().addMsgToChat(_nikName, _nikName, Answers[1]);
                    }



                }

                Chat = ChatLogModel.getInstance().getChat(_nikName);
                OnPropertyChanged(nameof(Chat));

                userMsg = "";
                OnPropertyChanged(nameof(userMsg));

            });
        }

        // Конструктор
        public MainWindowViewModel()
        {
            Contacts = ChatContactsModel.getInstance().getContacts();
            setupCommands();
 
        }

    }
}
