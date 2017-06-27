import React from 'react';
import expect from 'expect';
import expectJSX from 'expect-jsx';
import { getShallowComponent } from '../_helpers/shallow';
import SearchDropdown from '../../app/components/Search/SearchDropdown/index';

expect.extend(expectJSX);

describe('SearchDropdown component', () => {
  test('Should show `No result found` if there is message', () => {
    const Component = <SearchDropdown message='No result found' areResultsShown={true}/>;
    const actual = getShallowComponent(Component);
    const expected = <p className='search__noresults'>No result found</p>;
    expect(actual).toIncludeJSX(expected);
  });
});
