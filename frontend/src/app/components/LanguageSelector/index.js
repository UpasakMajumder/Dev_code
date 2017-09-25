import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import SVG from 'app.dump/SVG';
import LanguageDropdown from 'app.dump/LanguageDropdown';
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

  changeLanguage = (item) => {
    location.assign(item.url);
  };

  render() {
    const selectedLanguage = LANGUAGES.languages.filter(item => item.id === LANGUAGES.selectedId);

    const languagesComponent = this.props.languageSelector
      ? <LanguageDropdown languages={LANGUAGES.languages} changeLanguage={this.changeLanguage}/>
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
          <SVG name="small-arrow" className="nav-item--secondary" style={{
            width: '10px',
            height: '10px',
            marginLeft: '5px',
            rotate: this.props.languageSelector ? '180deg' : '0deg'
          }}/>
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
