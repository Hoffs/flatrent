import React, { ReactNode } from "react";
import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { getImageUrl } from "../../services/ApiUtilities";
import Styles from "./ImageCarousel.module.css";

interface IImageCarousel {
  imageIds: string[];
  wrapperClassName: string;
}

const ImageCarousel = (props: IImageCarousel) => {
  return (
    <Carousel className={Styles.carousel} showStatus={false} showThumbs={false} >
      {getCarouselImages(props.imageIds, props.wrapperClassName)}
    </Carousel>
  );
};

const getCarouselImages = (ids: string[], wrapperClassName: string): ReactNode[] => {
  return ids.map((id) => getImageUrl(id)).map((url, idx) =>
    (<div className={Styles.imageWrapper.concat(" ", wrapperClassName)}>
      <img className={Styles.image} key={ids[idx]} src={url} />
    </div>));
};

export default ImageCarousel;
