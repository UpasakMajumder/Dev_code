import React, { Component } from 'react';
import { connect } from 'react-redux';
import ReactPaginate from 'react-paginate';
import Spinner from '../../Spinner';
import { getUI, changePage, setPaginationLimit } from '../../../AC/searchPage';
import TemplateProduct from '../../TemplateProduct';
import { paginationFilter } from '../../../helpers/array';
import { getSearch } from '../../../helpers/location';

class SearchPageProducts extends Component {

  componentDidMount() {
    const { phrase } = getSearch();

    if (phrase) {
      this.props.getUI(phrase);
    } else {
      this.props.getUI('');
    }

    function changePaginationLimit(callback) {
      const width = window.innerWidth;
      if (width < 1200) {
        callback('productsPaginationLimit', 6);
        return;
      }

      callback('productsPaginationLimit', 8);
    }

    changePaginationLimit(this.props.setPaginationLimit);
    window.addEventListener('resize', () => changePaginationLimit(this.props.setPaginationLimit));
  }

  render() {
    const { filteredProducts, productsPage, productsLength, productsPaginationLimit } = this.props;

    const productList = filteredProducts.map((product) => {
      return (
        <div key={product.id} className="col-lg-4 col-xl-3">
          <TemplateProduct {...product} />
        </div>
      );
    });

    const pageCount = productsLength / productsPaginationLimit;

    const paginator = productsLength > productsPaginationLimit
      ? <ReactPaginate pageCount={pageCount}
                       pageRangeDisplayed={3}
                       marginPagesDisplayed={1}
                       onPageChange={(e) => { this.props.changePage(e.selected, 'productsPage'); }}
                       initialPage={productsPage}
                       previousClassName="pagination__item"
                       nextClassName="pagination__item"
                       containerClassName="pagination text--right list--unstyled"
                       pageClassName="pagination__item"
                       pageLinkClassName="pagination__page"
                       activeClassName="pagination__page--active" />
      : null;

    const content = (
      <div>
        <div className="row">
          {productList}
        </div>
        {paginator}
      </div>
    );

    return productsLength ? content : <Spinner />;
  }
}

export default connect((state) => {
  const { products, productsPage, productsPaginationLimit } = state.searchPage;

  const filteredProducts = paginationFilter(products, productsPage, productsPaginationLimit);

  const productsLength = products.length;

  return { filteredProducts, productsPage, productsLength, productsPaginationLimit };
}, {
  getUI,
  changePage,
  setPaginationLimit
})(SearchPageProducts);
