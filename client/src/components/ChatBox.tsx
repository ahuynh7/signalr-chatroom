import { ChatResponse } from '../App';
import Chat from './Chat';
import { ChangeEvent, FormEvent, useLayoutEffect, useRef, useState } from 'react';
import axios from "axios";

interface ChatRequest {
    username: string;
    message: string;
}

const Chatbox = ({messages}: {messages: ChatResponse[]}) => {
  const [inputValue, setInputValue] = useState("");
  const [charCount, setCharCount] = useState(250);
  const chatboxRef = useRef<HTMLDivElement | null>(null);

  useLayoutEffect(() => {
    if (chatboxRef.current) {
      chatboxRef.current.scrollTop = chatboxRef.current.scrollHeight;
    }
  }, [messages]);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const text = e.target.value;
    if (text.length <= 250) {
      setInputValue(text);
      setCharCount(250 - text.length);
    }
  };

  const handleSendMessage = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (inputValue.trim() !== '') {
      setInputValue('');
      setCharCount(250);
    
      //send post request to insert chat message
      let request: ChatRequest = {
        username: "user",
        message: inputValue
      }

      await axios.post(`${process.env.REACT_APP_API_URL}/message`, request);
    }
  };

  return (
    <div className="chatbox-container">
      <div className="chatbox-messages" ref={chatboxRef}>
        {messages.map((message, index) => (
          <Chat key={index} chat={message} />
        ))}
      </div>
      <form className="chatbox-input-container" onSubmit={handleSendMessage}>
        <input
          type="text"
          className="chatbox-input"
          placeholder="Type your message..."
          value={inputValue}
          onChange={handleInputChange}
          maxLength={250}
        />
        <div className="chatbox-footer">
          <span className="char-count">{charCount} characters remaining</span>
          <button type="submit" className="send-button">
            Send
          </button>
        </div>
      </form>
    </div>
  );
};

export default Chatbox;
