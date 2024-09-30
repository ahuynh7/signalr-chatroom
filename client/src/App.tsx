import { useEffect, useState } from 'react';
import './App.css';
import * as signalR from '@microsoft/signalr';
import Chatbox from './components/ChatBox';
import useFetchMessages from './hooks/useFetchMessages';

export interface ChatResponse {
  timestamp: number;
  username: string;
  message: string;
}

function App() {
  const { messages, setMessages } = useFetchMessages();
  const [connection, setConnection] = useState<signalR.HubConnection>();

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withAutomaticReconnect()
      .build();

    connection.baseUrl = `${process.env.REACT_APP_API_URL}/chatroom`;

    setConnection(connection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          connection.on("InsertChat", (datetime: string, user: string, message: string) => {
            let chat: ChatResponse = {
              timestamp: new Date(datetime).getTime(),//todo: temp
              username: user,
              message: message
            };

            setMessages((messages) => [...messages, chat]);
          });
        })
        .catch((error) => console.error(error));
    }
   }, [connection, setMessages]);

  return (
    <div className="App">
      <Chatbox messages={messages} />
    </div>
  );
}

export default App;
