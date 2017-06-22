import React, { Component } from 'react';
import { connect } from 'react-redux';
import SearchInput from './SearchInput';
import SearchDropdown from './SearchDropdown';
import { changeSearchQuery, closeDropdown, sendQuery } from '../../AC/search';

class Search extends Component {
  constructor() {
    super();

    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(query) {
    this.props.changeSearchQuery(query);
    if (query.length > 3) this.props.sendQuery(query);
    if (!query.length) this.props.closeDropdown();
  }

  render() {
    const { query, products, pages, message, areResultsShown } = this.props;

    const content = (
      <div>
        <SearchInput changeValue={this.handleChange}
                     closeDropdown={this.props.closeDropdown}
                     searchPageUrl={products ? products.url : undefined}
                     value={query} />
          <SearchDropdown areResultsShown={areResultsShown}
                          products={products}
                          pages={pages}
                          message={message} />
      </div>
    );

    return content;
  }
}

export default connect((state) => {
  const { search } = state;
  const { products, pages, message, query, areResultsShown } = search;
  return { query, products, pages, message, areResultsShown };
}, {
  changeSearchQuery,
  closeDropdown,
  sendQuery
})(Search);
