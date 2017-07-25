import React from 'react';
import Shallow from 'react-test-renderer/shallow';
import SearchProducts from '../../app/components/Search/SearchProducts';
import expect from 'expect';
import expectJSX from 'expect-jsx';

expect.extend(expectJSX);

describe('Products', () => {
  const products = {
    url: '#',
    items: [
      {
        id: 1,
        image: 'https://satyr.io/500-700x600-800',
        category: 'Category Name',
        title: 'Apr 16 17 Lamp post 2nd',
        url: '#',
        stock: {
          type: 'available',
          text: '150 pcs in stock'
        }
      }
    ]
  };

  test('Products should have Link', () => {
    const shallow = Shallow.createRenderer();
    shallow.render(<SearchProducts products={products} />);
    const actual = shallow.getRenderOutput();
    const expected = <a href="#">Show all products</a>;
    expect(actual).toIncludeJSX(expected);
  });
});
