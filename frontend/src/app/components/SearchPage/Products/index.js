import React, { Component } from 'react';
import { connect } from 'react-redux';
import Spinner from '../../Spinner';
import { getUI, changePage, setPaginationLimit } from '../../../AC/searchPage';
import TemplateProduct from '../../TemplateProduct';
import Pagination from '../../Pagination';
import { paginationFilter } from '../../../helpers/array';
import { getSearchObj } from '../../../helpers/location';

class SearchPageProducts extends Component {

  componentDidMount() {
    const { phrase } = getSearchObj();

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

    const content = (
      <div>
        <div className="row">
          {productList}
        </div>
        <Pagination pagesNumber={pageCount}
                    initialPage={0}
                    itemsOnPage={productsPaginationLimit}
                    itemsNumber={productsLength}
                    currPage={productsPage}
                    onPageChange={(e) => { this.props.changePage(e.selected, 'productsPage'); }}/>
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
