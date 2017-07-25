import React from 'react';
import Shallow from 'react-test-renderer/shallow';
import SearchDropdown from '../../app/components/Search/SearchDropdown/index';
import expect from 'expect';
import expectJSX from 'expect-jsx';

expect.extend(expectJSX);

describe('SearchDropdown component', () => {
  test('Should show `No result found` if there is message', () => {
    const shallow = Shallow.createRenderer();
    shallow.render(<SearchDropdown message='No result found' areResultsShown={true}/>);
    const actual = shallow.getRenderOutput();
    const expected = <p className='search__noresults'>No result found</p>;
    expect(actual).toIncludeJSX(expected);
  });
});
