import React from 'react';
import expect from 'expect';
import expectJSX from 'expect-jsx';
import { getShallowComponent } from '../_helpers/shallow';
import PageSearch from '../../app/components/Pages/Search';

expect.extend(expectJSX);

describe('PageSearch component', () => {
  test('PageSearch component', () => {
    const Component = <PageSearch url="#" title="Hello"/>;
    const actual = getShallowComponent(Component);
    const expected = <a href="#">Hello</a>;
    expect(actual).toIncludeJSX(expected);
  });
});
