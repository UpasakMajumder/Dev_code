import React from 'react';
import PropTypes from 'prop-types';
import Page from 'app.dump/Pages/Search';

const SearchPages = ({ pages }) => {
  const { url, items } = pages;

  const filteredItems = items.filter((item, index) => index < 3);
  const pagesList = filteredItems.map(item => <li key={item.id}><Page {...item} /></li>);

  return items.length
    ? (
      <div>
        <div className="search__header">
          <h2>Pages</h2>
          <a href={url}>Show all pages</a>
        </div>

        <ul className="search__result-pages">
          {pagesList}
        </ul>
      </div>
    )
    : null;
};

SearchPages.propTypes = {
  pages: PropTypes.shape({
    url: PropTypes.string,
    items: PropTypes.arrayOf(PropTypes.shape({
      url: PropTypes.string.isRequired,
      title: PropTypes.string.isRequired
    }))
  })
};

export default SearchPages;
