import {
  MoreVert,
  FavoriteBorder,
  Favorite,
  NotificationAdd,
  Share,
  Delete,
} from '@mui/icons-material';
import {
  Avatar,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  CardMedia,
  Checkbox,
  Grid,
  IconButton,
  Menu,
  MenuItem,
  Tooltip,
  Typography,
} from '@mui/material';
import React, { SyntheticEvent, useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../app/store/storeConfig';
import PostDetailsSidebar from './PostDetailsSidebar';
import {
  fetchPostAsync,
  followPostAsync,
  postSelectors,
  setLastViewPosts,
  setPost,
} from './postSlice';
import moment from 'moment';
import { dateTimeFormat, isFollowingThisPost } from '../../app/utils/utils';
import { toast } from 'react-toastify';
import SimpleImageSlider from 'react-simple-image-slider';
import { isArray } from 'lodash';
import PostSettingOptions from './PostSettingOptions';
import { Post } from 'app/models/post';

// Note: Post/Comment section reference: https://codesandbox.io/s/2393m2k5rj?file=/src/index.js

export default function PostDetails() {
  const dispatch = useAppDispatch();
  const { id } = useParams<{ id: string }>();
  const currentPost = useAppSelector((state) =>
    postSelectors.selectById(state, id)
  );
  // get current login user
  const { user } = useAppSelector((state) => state.auth);
  // const { lastViewPosts } = useAppSelector((state) => state.posts);
  const [postSetting, setPostSetting] = useState(false);

  const getPhotosFromCurrentPost = (post: Post) => {
    const images = post?.photos.map((photo) => {
      return { url: photo.url };
    });
    return images;
  };

  useEffect(() => {
    if (!currentPost) {
      dispatch(fetchPostAsync(id));
    }

    // handle fetch viewed posts history
    if (currentPost) {
      const { id, title, content, photos } = currentPost;
      const currentViewedPost = {
        id,
        title,
        content,
        photos,
        timestamp: Date.now(),
      };

      // get current total viewed posts
      const totalViewedPosts = JSON.parse(
        window.localStorage.getItem('lastViewedPosts') || '[]'
      );

      // destructuring current total viewed posts with newly viewed post
      currentViewedPost &&
        window.localStorage.setItem(
          'lastViewedPosts',
          JSON.stringify([currentViewedPost, ...Array.from(totalViewedPosts)])
        );

      //
      let lastViewedPosts = JSON.parse(
        window.localStorage.getItem('lastViewedPosts')! || '[]'
      );

      // slice viewed posts if  it's length is greater than 3
      if (Array.isArray(lastViewedPosts) && lastViewedPosts.length > 3)
        lastViewedPosts = lastViewedPosts.shift();

      lastViewedPosts && dispatch(setLastViewPosts(lastViewedPosts));
    }
  }, [id, dispatch]);

  // Note: this might be useless
  const [photoDetail, setPhotoDetail] = useState(currentPost?.photos[0].url);
  const onChangePhoto = (e: SyntheticEvent, photoIndex: number) => {
    setPhotoDetail(currentPost?.photos[photoIndex].url);
  };

  // post following logic
  const [isFollowing, setIsFollowing] = useState(
    isFollowingThisPost(user!, currentPost!)
  );
  const handleFollowPost = () => {
    dispatch(followPostAsync(currentPost!)).then(() => {
      console.log(isFollowingThisPost(user!, currentPost!));
      if (isFollowingThisPost(user!, currentPost!)) {
        setIsFollowing(false);
        toast.success('Unfollowing this post successfully');
        return;
      }
      setIsFollowing(true);
      toast.done('Started following this post');
    });
  };

  // just use to logging post is followed or not
  useEffect(() => {
    console.log(isFollowing);
  }, [isFollowing]);

  return (
    <Grid container columnSpacing={4}>
      <Grid item sm={10} xs={10}>
        <Card sx={{ margin: 2 }}>
          <CardHeader
            avatar={
              <Avatar sx={{ bgcolor: 'red' }} aria-label="recipe">
                {currentPost?.posterName[0].toUpperCase()}
              </Avatar>
            }
            action={
              user?.username === currentPost?.posterName ? (
                <PostSettingOptions currentPost={currentPost} />
              ) : (
                <></>
              )
            }
            title={currentPost?.title}
            subheader={`posted by ${currentPost?.posterName} | ${dateTimeFormat(
              currentPost?.createdAt!
            )}`}
          />
          <Menu
            id="demo-positioned-menu"
            aria-labelledby="demo-positioned-button"
            style={{ top: '30px' }}
            open={postSetting}
            onClose={(e) => setPostSetting(false)}
            anchorOrigin={{
              vertical: 'center',
              horizontal: 'center',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
          >
            <MenuItem component={Link} to={``}>
              Edit Post
            </MenuItem>
            <MenuItem>
              <IconButton>
                <Delete />
              </IconButton>
            </MenuItem>
          </Menu>
          <CardContent>
            <Typography variant="body2" color="text.secondary">
              {currentPost?.content}
            </Typography>
            <br />
            <Typography variant="h6" color="Highlight">
              Post location
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {currentPost?.postLocation.location}
              <p>
                <a
                  href={`https://map.kakao.com/?urlX=${currentPost?.postLocation.longtitute}&urlY=${currentPost?.postLocation.latitude}&urlLevel=3&itemId=23891175&q=${currentPost?.postLocation.location}&srcid=23891175&map_type=TYPE_MAP`}
                >
                  View location details on Map
                </a>
              </p>
            </Typography>
          </CardContent>
          <CardMedia component="div">
            <SimpleImageSlider
              width={896}
              height={504}
              images={
                isArray(getPhotosFromCurrentPost(currentPost!))
                  ? getPhotosFromCurrentPost(currentPost!)
                  : []
              }
              showBullets={true}
              showNavs={true}
            />
          </CardMedia>
          <CardActions disableSpacing>
            <Tooltip title="Like">
              <IconButton aria-label="add to favorites">
                <Checkbox
                  icon={<FavoriteBorder />}
                  checkedIcon={<Favorite sx={{ color: 'red' }} />}
                />
              </IconButton>
            </Tooltip>
            <Tooltip title="Share">
              <IconButton aria-label="share">
                <Share />
              </IconButton>
            </Tooltip>
            <Tooltip title={isFollowing ? `Unfollowing` : `Following`}>
              <IconButton
                onClick={() => handleFollowPost()}
                aria-label="following"
              >
                <NotificationAdd />
              </IconButton>
            </Tooltip>
          </CardActions>
        </Card>
      </Grid>
      <Grid item sm={2}>
        <PostDetailsSidebar
          postPhotos={currentPost?.photos}
          onChangePhoto={onChangePhoto}
        />
      </Grid>
    </Grid>
  );
}
