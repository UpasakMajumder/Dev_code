import React, { Component } from 'react';
import { connect } from 'react-redux';
import SearchInput from './SearchInput';
import SearchDropdown from './SearchDropdown';
import { changeSearchQuery, closeDropdown, sendQuery } from '../../AC/search';

class Search extends Component {
  state = {
    workingProcess: 0
  };

  handleChange = (event) => {
    const { target } = event;
    const { value: query } = target;
    const { changeSearchQuery, sendQuery, closeDropdown } = this.props;
    const { workingProcess } = this.state;

    changeSearchQuery(query);

    if (query.length > 3) {
      clearTimeout(workingProcess);
      const workingProcessId = setTimeout(() => {
        sendQuery(query);
      }, 1000);
      this.setState({ workingProcess: workingProcessId });
    }

    if (!query.length) closeDropdown();
  };

  render() {
    const { query, products, pages, message, areResultsShown, closeDropdown } = this.props;

    return (
      <div>
        <SearchInput changeValue={this.handleChange}
                     closeDropdown={closeDropdown}
                     searchPageUrl={products ? products.url : undefined}
                     value={query}
        />

        <SearchDropdown areResultsShown={areResultsShown}
                        products={products}
                        pages={pages}
                        message={message}
        />
      </div>
    );
  }
}

export default connect((state) => {
  const { search } = state;
  const { products, pages, message, query, areResultsShown } = search;
  return { products, pages, message, query, areResultsShown };
}, {
  changeSearchQuery,
  closeDropdown,
  sendQuery
})(Search);
