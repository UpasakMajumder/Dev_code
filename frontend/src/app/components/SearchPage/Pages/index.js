import React, { Component } from 'react';
import { connect } from 'react-redux';
import ReactPaginate from 'react-paginate';
import Spinner from '../../Spinner';
import Page from './Page';
import { changePage } from '../../../AC/searchPage';
import { paginationFilter } from '../../../helpers/array';

class SearchPagePages extends Component {
  render() {
    const { filteredPages, pagesPage, pagesLength } = this.props;

    const pageList = filteredPages.map(page => <Page key={page.id} {...page} />);

    const pageCount = pagesLength / 6;

    const paginator = pagesLength > 6
      ? <ReactPaginate pageCount={pageCount}
                       pageRangeDisplayed={3}
                       marginPagesDisplayed={1}
                       onPageChange={(e) => { this.props.changePage(e.selected, 'pagesPage'); }}
                       initialPage={pagesPage}
                       previousClassName="pagination__item"
                       nextClassName="pagination__item"
                       containerClassName="pagination text--right list--unstyled"
                       pageClassName="pagination__item"
                       pageLinkClassName="pagination__page"
                       activeClassName="pagination__page--active" />
      : null;

    const content = (
      <div>
        {pageList}
        {paginator}
      </div>
    );

    return pagesLength ? content : <Spinner/>;
  }
}

export default connect((state) => {
  const { pages, pagesPage } = state.searchPage;

  const filteredPages = paginationFilter(pages, pagesPage, 6);

  const pagesLength = pages.length;

  return { filteredPages, pagesPage, pagesLength };
}, {
  changePage
})(SearchPagePages);
