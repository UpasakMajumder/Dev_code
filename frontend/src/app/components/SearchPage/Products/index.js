import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Alert from 'app.dump/Alert';
import Spinner from '../../Spinner';
import Pagination from '../../Pagination';
import TemplateProduct from '../../TemplateProduct';
import { getSearchObj } from '../../../helpers/location';
import { paginationFilter } from '../../../helpers/array';
import { getUI, changePage, setPaginationLimit } from '../../../AC/searchPage';

class SearchPageProducts extends Component {
  static propTypes = {
    productsLength: PropTypes.PropTypes.number.isRequired,
    productsPaginationLimit: PropTypes.number.isRequired,
    productsPage: PropTypes.PropTypes.number.isRequired,
    getAllResults: PropTypes.bool.isRequired,
    changePage: PropTypes.func.isRequired,
    filteredProducts: PropTypes.arrayOf(PropTypes.shape({
      title: PropTypes.string.isRequired,
      stock: PropTypes.shape({
        type: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired
      }),
      useTemplateBtn: PropTypes.shape({
        text: PropTypes.string.isRequired,
        url: PropTypes.string.isRequired
      }).isRequired,
      breadcrumbs: PropTypes.arrayOf(PropTypes.string).isRequired,
      isFavourite: PropTypes.bool,
      imgUrl: PropTypes.string
    }))
  };

  componentDidMount() {
    const { phrase } = getSearchObj();
    const { getUI, setPaginationLimit } = this.props;

    if (phrase) {
      getUI(phrase);
    } else {
      getUI('');
    }

    function changePaginationLimit(callback) {
      const width = window.innerWidth;
      if (width < 1200) {
        callback('productsPaginationLimit', 6);
        return;
      }

      callback('productsPaginationLimit', 8);
    }

    changePaginationLimit(setPaginationLimit);
    window.addEventListener('resize', () => changePaginationLimit(setPaginationLimit));
  }

  render() {
    const { filteredProducts, productsPage, productsLength, productsPaginationLimit, changePage, getAllResults } = this.props;

    const productList = filteredProducts.map((product) => {
      return <div key={product.id} className="col-lg-4 col-xl-3"><TemplateProduct {...product} /></div>;
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
                    onPageChange={(e) => { changePage(e.selected, 'productsPage'); }}
        />
      </div>
    );

    const plug = getAllResults
      ? <Alert type="info" text="No products found"/>
      : <Spinner />;

    return productsLength ? content : plug;
  }
}

export default connect((state) => {
  const { products, productsPage, productsPaginationLimit, getAllResults } = state.searchPage;

  const filteredProducts = paginationFilter(products, productsPage, productsPaginationLimit);

  const productsLength = products.length;

  return { filteredProducts, productsPage, productsLength, productsPaginationLimit, getAllResults };
}, {
  getUI,
  changePage,
  setPaginationLimit
})(SearchPageProducts);
