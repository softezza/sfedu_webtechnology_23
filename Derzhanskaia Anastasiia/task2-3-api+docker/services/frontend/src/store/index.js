import { createStore } from "vuex";

import goals from './modules/goals';
import users from './modules/users';

export default createStore({
  modules: {
    goals,
    users,
  }
});