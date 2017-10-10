import React from 'react';
import expect from 'expect';
import expectJSX from 'expect-jsx';
import { getShallowComponent } from '../_helpers/shallow';
import Alert from '../../app/components/_dump/Alert';

expect.extend(expectJSX);

describe('Alert component', () => {
  test('Alert should be rendered with text Hello and class alert--info', () => {
    const Component = <Alert type="info" text="Hello"/>;
    const actual = getShallowComponent(Component);
    const expected = (
      <div className={'alert--info alert--full alert--smaller isOpen'}>
        <span>Hello</span>
      </div>
    );
    expect(actual).toEqualJSX(expected);
  });
});
