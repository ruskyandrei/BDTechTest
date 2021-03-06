import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Search from './components/Search';

import './custom.css'


export default () => (
    <Layout>
        <Route exact path='/' component={Search} />
    </Layout>
);
