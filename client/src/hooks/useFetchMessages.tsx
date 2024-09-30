import { useEffect, useState } from "react";
import { ChatResponse } from "../App";
import axios from "axios";

const getMessages = async () => 
  await axios.get(`${process.env.REACT_APP_API_URL}/messages`)

const useFetchMessages = () => {
  const [messages, setMessages] = useState<ChatResponse[]>([]);

  useEffect(() => {
    //fetch past messages
    getMessages()
      .then((response) => {
        if (messages.length === 0)
          setMessages(response.data.reverse());
      });
  }, [messages.length]);

  return { messages, setMessages };
}

export default useFetchMessages;