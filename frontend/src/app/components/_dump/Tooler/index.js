import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* 3rd part */
import { Tooltip } from 'react-tippy';

const Tooler = ({ text, html, position = 'right', type = 'default', symbol = 'question' }) => {
  return (
    <Tooltip
      title={text}
      html={<div dangerouslySetInnerHTML={{ __html: html }} />}
      position={position}
      animation="fade"
      arrow={true}
      theme="dark"
    >
      <div className={`tooler tooler--${type}`}>
        <SVG name="question-mark" className="icon-question"/>
      </div>
    </Tooltip>
  );
};

Tooler.propTypes = {
  text: PropTypes.string,
  html: PropTypes.string,
  type: PropTypes.string.isRequired,
  symbol: PropTypes.string,
  position: PropTypes.string
};

export default Tooler;
