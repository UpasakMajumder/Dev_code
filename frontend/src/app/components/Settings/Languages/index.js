import React, { Component } from 'react';
/* components */
import TextInput from 'app.dump/Form/TextInput';
import LanguageDropdown from 'app.dump/LanguageDropdown';
import SVG from 'app.dump/SVG';
/* globals */
import { LANGUAGES } from 'app.globals';

class SettingLanguages extends Component {
  state = {
    showLanguages: false,
    selectedId: LANGUAGES.selectedId
  };

  toogleDropdown = () => {
    this.setState((prev) => {
      return {
        showLanguages: !prev.showLanguages
      };
    });
  };

  changeSelectedLanguage = (item) => {
    this.toogleDropdown();
    this.setState({ selectedId: item.id });
  };

  render() {
    const selectedItem = LANGUAGES.languages.filter(item => this.state.selectedId === item.id)[0];

    const languagesComponent = this.state.showLanguages
      ? <LanguageDropdown shadow={true} languages={LANGUAGES.languages} changeLanguage={this.changeSelectedLanguage}/>
      : null;

    return (
      <div className="settings__block">
        <div className="settings__item">
          <h2>{LANGUAGES.title}</h2>
          <p>{LANGUAGES.description}</p>
          <div className="language-selector language-selector--short">
            <TextInput
              readOnly={true}
              placeholder={LANGUAGES.placeholder}
              value={selectedItem.language}
              className="language-selector__input"
              onClick={this.toogleDropdown}
            />
            <SVG name="small-arrow" className="language-selector__icon" style={{
              rotate: this.state.showLanguages ? '180deg' : '0deg'
            }}/>
            {languagesComponent}
          </div>
          <a className="btn-action" href={selectedItem.url}>{LANGUAGES.buttonTitle}</a>
        </div>
      </div>
    );
  }
}

export default SettingLanguages;
