import { useEffect, useState } from 'react';
import './App.css';
import axios from "axios";
import * as signalR from '@microsoft/signalr';
import Chatbox from './components/ChatBox';

export interface ChatResponse {
  timestamp: number;
  username: string;
  message: string;
}

const getMessages = async () => 
  await axios.get(`http://${process.env.REACT_APP_API_URL}/messages`)

function App() {
  const [connection, setConnection] = useState<signalR.HubConnection>();
  const [messages, setMessages] = useState<ChatResponse[]>([]);

  useEffect(() => {
    //establish connection signalr hub server
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_API_URL}/chatroom`)
      .withAutomaticReconnect()
      .build();

    setConnection(connection);
  }, []);

  useEffect(() => {
    //fetch past messages
    getMessages()
      .then((response) => {
        if (messages.length === 0)
          setMessages(response.data.reverse());
      });
  }, [messages.length]);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          connection.on("InsertChat", (timestamp: string, user: string, message: string) => {
            let chat: ChatResponse = {
              timestamp: new Date(timestamp).getTime(),
              username: user,
              message: message
            };

            setMessages((messages) => [...messages, chat]);
          });
        })
        .catch((error) => console.error(error));
    }
   }, [connection]);
  return (
    <div className="App">
      <Chatbox messages={messages} />
    </div>
  );
}

export default App;
