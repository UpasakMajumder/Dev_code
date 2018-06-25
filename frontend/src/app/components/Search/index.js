import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* ac */
import { changeSearchQuery, closeDropdown, sendQuery } from 'app.ac/search';
/* local components */
import SearchInput from './SearchInput';
import SearchDropdown from './SearchDropdown';

class Search extends Component {
  state = {
    workingProcess: 0
  };

  static propTypes = {
    changeSearchQuery: PropTypes.func.isRequired,
    areResultsShown: PropTypes.bool.isRequired,
    closeDropdown: PropTypes.func.isRequired,
    pressedEnter: PropTypes.bool.isRequired,
    products: PropTypes.object.isRequired,
    sendQuery: PropTypes.func.isRequired,
    message: PropTypes.string.isRequired,
    pages: PropTypes.object.isRequired,
    query: PropTypes.string.isRequired
  };

  handleChange = (event) => {
    const { target } = event;
    const { value: query } = target;
    const { changeSearchQuery, sendQuery, closeDropdown } = this.props;
    const { workingProcess } = this.state;

    changeSearchQuery(query);

    if (query.length > 2) {
      clearTimeout(workingProcess);
      const workingProcessId = setTimeout(() => {
        sendQuery(query);
      }, 1000);
      this.setState({ workingProcess: workingProcessId });
    }

    if (!query.length) {
      clearTimeout(workingProcess);
      closeDropdown();
    }
  };

  redirectUserToResultPage = (e) => {
    const { workingProcess } = this.state;
    const { query, sendQuery } = this.props;

    if (e.keyCode === 13) {
      clearTimeout(workingProcess);
      sendQuery(query, true);
      e.preventDefault();
    }
  };

  componentWillReceiveProps(nextProps) { // eslint-disable-line class-methods-use-this
    const { pressedEnter, products, pages } = nextProps;
    if (pressedEnter) {
      let urlTohResultPage = '#';
      if (products) {
        if (products.url) {
          urlTohResultPage = products.url;
        } else if (pages) {
          if (pages.url) {
            urlTohResultPage = pages.url;
          }
        }
      }

      location.href = urlTohResultPage;
    }
  }

  render() {
    const { query, products, pages, message, areResultsShown, closeDropdown } = this.props;

    return (
      <div>
        <SearchInput changeValue={this.handleChange}
                     closeDropdown={this.props.closeDropdown}
                     redirectUserToResultPage={this.redirectUserToResultPage}
                     value={query} />
          <SearchDropdown areResultsShown={areResultsShown}
                          products={products}
                          pages={pages}
                          message={message} />
      </div>
    );
  }
}

export default connect((state) => {
  const { search } = state;
  const { products, pages, message, query, areResultsShown, pressedEnter } = search;
  return { query, products, pages, message, areResultsShown, pressedEnter };
}, {
  changeSearchQuery,
  closeDropdown,
  sendQuery
})(Search);
