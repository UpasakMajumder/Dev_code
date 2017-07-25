import React from 'react';
import PropTypes from 'prop-types';
import SVG from '../../SVG';

const SearchInput = ({ changeValue, closeDropdown, value, searchPageUrl, redirectUserToResultPage }) => {
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
               onKeyDown={(e) => { redirectUserToResultPage(e); }}
               value={value}/>
        {closer}
      </div>
    </div>
  );
};

SearchInput.propTypes = {
  value: PropTypes.string.isRequired,
  closeDropdown: PropTypes.func.isRequired,
  changeValue: PropTypes.func.isRequired
};

export default SearchInput;
