import Vue from 'vue';
import VeeValidate from 'vee-validate';

import { store } from './store';
import { router } from './helpers';
import App from './components/App';

Vue.use(VeeValidate);

new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
});
