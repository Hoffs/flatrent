import React from "react";
import { Redirect, Route, RouteProps } from "react-router-dom";
import UserService from "../../services/UserService";
import { Authentication } from "../../Routes";

interface IRoleRouteProps extends RouteProps {
  redirect: string;
  authenticated: Authentication;
  allowedRoles?: number[];
}

const RoleRoute = (props: IRoleRouteProps) => {
  let satisfiesRoles = true;
  if (props.allowedRoles !== undefined && props.allowedRoles.length > 0) {
    satisfiesRoles = UserService.hasRoles(...props.allowedRoles);
  }

  if (satisfiesRoles && UserService.satisfiesAuthentication(props.authenticated)) {
    return <Route {...props} />;
  } else {
    const renderComponent = () => <Redirect to={props.redirect} />;
    return <Route {...props} component={renderComponent} />;
  }
};

export default RoleRoute;
