import React, { Component } from 'react';
import Page from '../../Pages/Search';

class SearchPages extends Component {
  render() {
    const { pages } = this.props;
    const { url, items } = pages;

    const filteredItems = items.filter((item, index) => index < 3);
    const pagesList = filteredItems.map(item => <li key={item.id}><Page {...item} /></li>);

    const content = items.length
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

    return content;
  }
}

export default SearchPages;
