import PropTypes from 'prop-types';
import { ChatResponse } from '../App';

const Chat = ({chat}: {chat: ChatResponse}) => {
  const { timestamp, username, message } = chat;

  return (
    <div className="chat-message">
      <div className="chat-message-header">
        <span className="chat-message-username">{username}</span>
        <span className="chat-message-timestamp">
          {new Date(timestamp).toLocaleTimeString()}
        </span>
      </div>
      <div className="chat-message-content">{message}</div>
    </div>
  );
};

Chat.propTypes = {
  chat: PropTypes.shape({
    timestamp: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
    username: PropTypes.string.isRequired,
    message: PropTypes.string.isRequired,
  }).isRequired,
};

export default Chat;