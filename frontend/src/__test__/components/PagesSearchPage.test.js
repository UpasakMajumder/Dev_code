import React from 'react';
import expect from 'expect';
import expectJSX from 'expect-jsx';
import { getShallowComponent } from '../_helpers/shallow';
import PagesSearchPage from '../../app/components/dump/Pages/SearchPage';

expect.extend(expectJSX);

describe('PageSearchPage component', () => {
  test('PageSearchPage should be render with title, url and describtion', () => {
    const text = '0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9';
    const Component = <PagesSearchPage url="#" title="Hi" text={text} />;
    const actual = getShallowComponent(Component);
    const expected = (
      <div className="search-result__page">
        <h3>
          <a href="#">Hi</a>
        </h3>
        <p>0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2...</p>
      </div>
    );
    expect(actual).toEqualJSX(expected);
  });

  test('PageSearchPage should be render with title, url without description', () => {
    const Component = <PagesSearchPage url="#" title="Hi" />;
    const actual = getShallowComponent(Component);
    const expected = (
      <div className="search-result__page">
        <h3>
          <a href="#">Hi</a>
        </h3>
      </div>
    );
    expect(actual).toEqualJSX(expected);
  });
});
