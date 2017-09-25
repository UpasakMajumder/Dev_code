import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import SVG from 'app.dump/SVG';
/* ac */
import { showLanguages, hideLanguages } from 'app.ac/languageSelector';
/* globals */
import { LANGUAGES } from 'app.globals';

class LanguageSelector extends Component {
  static propTypes = {
    languageSelector: PropTypes.bool.isRequired,
    showLanguages: PropTypes.func.isRequired,
    hideLanguages: PropTypes.func.isRequired
  };

  render() {
    const selectedLanguage = LANGUAGES.languages.filter(item => item.id === LANGUAGES.selectedId);

    const languages = LANGUAGES.languages.map((item) => {
      return (
        <li className="language-selector__item" key={item.id}>
          <a href={item.url}>{item.language}</a>
        </li>
      );
    });

    const languagesComponent = this.props.languageSelector
      ?
      (
        <ul className="language-selector__list language-selector__list--absolute">
          {languages}
        </ul>
      )
      : null;

    return (
      <div
        className="language-selector"
        onMouseEnter={() => this.props.showLanguages(true)}
        onMouseLeave={() => this.props.hideLanguages(true)}
      >
        <div className="nav-link">
          <SVG name="grid-world"/>
          <span>{selectedLanguage[0].language}</span>
          <SVG name="small-arrow" className="icon-chevron ml-1"/>
        </div>
        {languagesComponent}
      </div>
    );
  }
}

export default connect((state) => {
  const { languageSelector } = state;
  return { languageSelector };
}, {
  showLanguages,
  hideLanguages
})(LanguageSelector);
