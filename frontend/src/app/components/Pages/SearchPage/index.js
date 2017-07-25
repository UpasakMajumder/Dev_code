import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { cutWords } from '../../../helpers/string';

export default class Page extends Component {
  render() {
    const { url, title, text } = this.props;

    const descriptionText = text ? <p>{cutWords(text, 24)}</p> : null;

    return (
      <div className="search-result__page">
        <h3>
          <a href={url}>{title}</a>
        </h3>
        {descriptionText}
      </div>
    );
  }
}

Page.propTypes = {
  url: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  text: PropTypes.string
};
