import React, { Component } from 'react';
import { connect } from 'react-redux';
import SearchInput from './SearchInput';
import SearchDropdown from './SearchDropdown';
import { changeSearchQuery, closeDropdown, sendQuery } from '../../AC/search';

class Search extends Component {
  constructor() {
    super();

    this.state = {
      workingProcess: 0
    };

    this.handleChange = this.handleChange.bind(this);
    this.redirectUserToResultPage = this.redirectUserToResultPage.bind(this);
  }

  handleChange(event) {
    const { target } = event;
    const { value: query } = target;

    this.props.changeSearchQuery(query);

    if (query.length > 3) {
      clearTimeout(this.state.workingProcess);
      const workingProcess = setTimeout(() => {
        this.props.sendQuery(query);
      }, 1000);
      this.setState({ workingProcess });
    }

    if (!query.length) this.props.closeDropdown();
  }

  redirectUserToResultPage(e) {
    const { workingProcess } = this.state;
    const { query } = this.props;

    if (e.keyCode === 13) {
      clearTimeout(workingProcess);
      this.props.sendQuery(query, true);
      e.preventDefault();
    }
  }

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
    const { query, products, pages, message, areResultsShown } = this.props;

    const content = (
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

    return content;
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
