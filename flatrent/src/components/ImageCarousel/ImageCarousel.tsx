import React, { ReactNode, SyntheticEvent } from "react";
import ContentLoader from "react-content-loader";
import { Carousel } from "react-responsive-carousel";
// tslint:disable-next-line: no-submodule-imports
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { getImageUrl } from "../../services/ApiUtilities";
import { joined } from "../../utilities/Utilities";
import Styles from "./ImageCarousel.module.css";

interface IImageCarousel {
  imageIds?: string[];
  className?: string;
  wrapperClassName: string;
}

// tslint:disable: jsx-no-multiline-js
const ImageCarousel = (props: IImageCarousel) => {
  const className = props.className ? props.className : "";
  return (
    <Carousel className={joined(Styles.carousel, className)} showStatus={false} showThumbs={false} >
      {props.imageIds === undefined
        ? (<ImageLoader className={props.wrapperClassName} />)
        : getCarouselImages(props.imageIds, props.wrapperClassName)
      }
    </Carousel>
  );
};

const setImageToDefault = (evt: SyntheticEvent<HTMLImageElement, Event>) => {
  evt.currentTarget.setAttribute("src", "/placeholder.svg");
  evt.currentTarget.setAttribute("style",
    "height: fit-content;width: 300px;min-width: fit-content;min-height: fit-content;");
};

const getCarouselImages = (ids: string[], wrapperClassName: string): ReactNode[] => {
  return ids.map((id) => getImageUrl(id)).map((url, idx) =>
    (<div key={ids[idx]} className={Styles.imageWrapper.concat(" ", wrapperClassName)}>
      <img key={ids[idx]} className={Styles.image} src={url} onError={setImageToDefault} />
    </div>));
};

const ImageLoader = (props: { className: string }) => (
  <ContentLoader
    style={{width: "100%", height: "100%"}}
    height={100}
    width={100}
    speed={2}
    primaryColor="#f3f3f3"
    secondaryColor="#ecebeb"
  >
    <rect x="0" y="0" rx="0" ry="0" width="100" height="100" />
  </ContentLoader>
);

export default ImageCarousel;
