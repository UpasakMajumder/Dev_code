import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import SVG from 'app.dump/SVG';

class Products extends Component {
  static propTypes = {
    itemsNumber: PropTypes.number.isRequired
  };

  render() {

    //TODO UI
    const response = {
      "success": true,
      "payload": {
        "categories": [
          {
            "id": 21,
            "imageUrl": "http://localhost:56000/KDA/media/Kadena/Development/Brands/logo_intertherm.png?ext=.png",
            "title": "Product category 2",
            "url": "/Products/Product-category-1/Product-category-2"
          }
        ],
        "products": [
          {
            "id": 59,
            "imageUrl": "http://localhost:56000/getmetafile/539a1463-2f3e-463b-bb27-e4c9b3a13ac2/rhc_packaged_systems",
            "title": "Chilli product",
            "url": "/Products/Product-category-1/Chilli-product",
            "isFavorite": false
          },
          {
            "id": 83,
            "imageUrl": "http://localhost:56000/getmetafile/3dce5fb5-31dc-47a7-9831-e32c02e3a06a/envelope-silhouette-2",
            "title": "Static product",
            "url": "/Products/Product-category-1/Static-product",
            "isFavorite": false
          },
          {
            "id": 84,
            "imageUrl": "http://localhost:56000/getmetafile/263efac7-55f9-4378-bfb3-811792a19a89/my-product",
            "title": "POD static product",
            "url": "/Products/Product-category-1/POD-static-product",
            "isFavorite": true
          },
          {
            "id": 85,
            "imageUrl": "http://localhost:56000/getmetafile/1f8c87c3-6a69-4243-a96c-43a1826548ed/product_box",
            "title": "Mailing templated product",
            "url": "/Products/Product-category-1/Mailing-templated-product",
            "isFavorite": false
          },
          {
            "id": 86,
            "imageUrl": "http://localhost:56000/getmetafile/e79d1199-3cd7-4013-bcd9-7cc47c37dedd/licence-app",
            "title": "Templated product",
            "url": "/Products/Product-category-1/Templated-product",
            "isFavorite": true
          },
          {
            "id": 1088,
            "imageUrl": "",
            "title": "New Product for Testing",
            "url": "/Products/Product-category-1/New-Product-for-Testing",
            "isFavorite": false
          }
        ],
      },
      "errorMessage": null
    };


    console.log('response', response);


    /*return (
      <div>
        pam 2 pa dam {response.payload.categories.length}

        { response.payload.categories.map((category) => (
          <div className="col-lg-4 col-xl-3">
            <div className="category__title">
              <h2>{category.title}</h2>
            </div>
          </div>
        )) }
      </div>
    );*/

    const bgImage = "http://satyr.io/250-500x150-300?";


    return (
      <div className="row">
        {/* TODO add loading spinner */}


        { response.payload.categories.map((category) => (
          <div key={category.id} className="col-lg-4 col-xl-3">
            <a href={category.url} className="category">
              <div
                className="category__picture"
                style={{ backgroundImage: `url(${category.imageUrl})` }}>
              </div>
              <div className="category__title">
                <h2>{category.title}</h2>
              </div>
            </a>
          </div>
        )) }

        { response.payload.products.map((product) => (
          <div key={product.id} className="col-lg-4 col-xl-3">
            {/*TODO favorite */}
            <div className="template__favourite js-collapse js-tooltip" data-tooltip-placement="right" title="Set product as favorite">
              <div className="js-toggle">
                {product.isFavorite ?
                  <SVG name="star--filled" className="template__icon--filled icon-star"/>
                  : <SVG name="star--unfilled" className="template__icon--unfilled icon-star"/>
                }
              </div>
            </div>

            <a href={product.url} className="category">
              <div
                className="category__picture"
                style={{ backgroundImage: `url(${bgImage || product.imageUrl})` }}>
              </div>
              <div className="category__title">
                <h2>{product.title}</h2>
              </div>
            </a>
          </div>
        )) }
      </div>
    );
  }
}

export default connect((state) => {
  const { cartPreview } = state;
  const itemsNumber = cartPreview.items.length;

  return { itemsNumber };
}, {})(Products);
