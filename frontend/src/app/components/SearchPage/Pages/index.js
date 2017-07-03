import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Alert from 'app.dump/Alert';
import Pagination from '../../Pagination';
import Spinner from '../../Spinner';
import Page from '../../Pages/SearchPage/index';
import { changePage } from '../../../AC/searchPage';
import { paginationFilter } from '../../../helpers/array';

class SearchPagePages extends Component {
  static propTypes = {
    /* store */
    getAllResults: PropTypes.bool.isRequired,
    pagesLength: PropTypes.number.isRequired,
    pagesPage: PropTypes.number.isRequired,
    changePage: PropTypes.func.isRequired,
    filteredPages: PropTypes.arrayOf(PropTypes.shape({
      title: PropTypes.string.isRequired,
      url: PropTypes.string.isRequired,
      text: PropTypes.string
    })).isRequired
  };

  render() {
    const { filteredPages, pagesPage, pagesLength, changePage, getAllResults } = this.props;

    const pageList = filteredPages.map(page => <Page key={page.id} {...page} />);

    const pageCount = pagesLength / 6;

    const content = (
      <div>
        {pageList}
        <Pagination pagesNumber={pageCount}
                    initialPage={0}
                    itemsOnPage={6}
                    itemsNumber={pagesLength}
                    currPage={pagesPage}
                    onPageChange={(e) => { changePage(e.selected, 'pagesPage'); }}/>
      </div>
    );

    const plug = getAllResults
      ? <Alert type="info" text="No pages found"/>
      : <Spinner />;

    return pagesLength ? content : plug;
  }
}

export default connect((state) => {
  const { pages, pagesPage, getAllResults } = state.searchPage;

  const filteredPages = paginationFilter(pages, pagesPage, 6);
  const pagesLength = pages.length;

  return { filteredPages, pagesPage, pagesLength, getAllResults };
}, {
  changePage
})(SearchPagePages);
