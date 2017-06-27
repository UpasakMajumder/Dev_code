import React from 'react';
import PropTypes from 'prop-types';
import SVG from '../../SVG';

const SearchInput = ({ changeValue, closeDropdown, value, searchPageUrl }) => {
  const redirectUser = (e) => {
    if (e.keyCode === 13) {
      if (searchPageUrl) location.href = searchPageUrl;
      e.preventDefault();
    }
  };

  const closer = value
    ? <button onClick={closeDropdown} type="button" className="search__closer">
      <SVG name="cross-xl" className="icon-close-search"/>
    </button>
    : null;

  return (
    <div className="search__input">
      <div className="input__wrapper">
        <input type="text"
               className="input__text"
               placeholder="Search"
               onChange={(e) => { changeValue(e); }}
               onKeyDown={(e) => { redirectUser(e); }}
               value={value}/>
        {closer}
      </div>
    </div>
  );
};

SearchInput.propTypes = {
  closeDropdown: PropTypes.func.isRequired,
  changeValue: PropTypes.func.isRequired,
  value: PropTypes.string.isRequired,
  searchPageUrl: PropTypes.string
};

export default SearchInput;
