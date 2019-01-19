import React from "react";
import { Redirect, Route, RouteProps } from "react-router-dom";
import UserService from "../../services/UserService";

interface IRoleRouteProps extends RouteProps {
  redirect: string;
  authenticated: boolean;
  allowedRoles?: string[];
}

const RoleRoute = (props: IRoleRouteProps) => {
  const loggedIn = UserService.isLoggedIn();
  let satisfiesRoles = true;
  if (props.allowedRoles !== undefined && props.allowedRoles.length > 0) {
    satisfiesRoles = UserService.hasRoles(...props.allowedRoles);
  }
  const satisfiesAuth = loggedIn === props.authenticated;

  if (satisfiesRoles && satisfiesAuth) {
    return <Route {...props} />;
  } else {
    const renderComponent = () => <Redirect to={props.redirect} />;
    return <Route {...props} component={renderComponent} />;
  }
};

export default RoleRoute;
