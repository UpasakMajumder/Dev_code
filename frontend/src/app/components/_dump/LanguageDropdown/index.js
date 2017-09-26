import React from 'react';
import PropTypes from 'prop-types';

const LanguageDropdown = (props) => {
  const { languages, changeLanguage, shadow } = props;

  const languagesList = languages.map((item, i) => {
    return (
      <li
        className='language-selector__item'
        key={i}
        onClick={() => changeLanguage(item)}
      >
        {item.language}
      </li>
    );
  });

  return (
    <ul
      className={`language-selector__list language-selector__list--absolute ${shadow && 'language-selector__list--shadow'}`}>
      {languagesList}
    </ul>
  );
};

LanguageDropdown.propTypes = {
  languages: PropTypes.arrayOf(PropTypes.shape({
    language: PropTypes.string.isRequired
  }).isRequired).isRequired,
  shadow: PropTypes.bool,
  changeLanguage: PropTypes.func.isRequired
};

export default LanguageDropdown;
