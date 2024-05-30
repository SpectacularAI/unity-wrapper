#include "../include/spectacularAI/unity/depthai.hpp"

#include <spectacularAI/output.hpp>
#include <iostream>
#include <sstream>
#include <vector>

int main(int argc, char *argv[]) {
    ConfigurationWrapper config;
    config.lowLatency = true;

    // SLAM callback
    callback_t_mapper_output onMapperOutput = [](const MapperOutputWrapper* mapperOutput) {
        const int64_t* updatedKeyFrames;
        int32_t nUpdatedKeyFrames = sai_mapper_output_get_updated_key_frames(mapperOutput, &updatedKeyFrames);
        if (sai_mapper_output_get_final_map(mapperOutput)) {
            std::cout << "SLAM: final map: " << nUpdatedKeyFrames << std::endl;
        } else {
            std::cout << "SLAM: update map: " << nUpdatedKeyFrames << std::endl;
        }
        sai_mapper_output_release(mapperOutput); // must release memory!
    };

    PipelineWrapper* pipeline = sai_depthai_pipeline_build(&config, nullptr, 0, onMapperOutput);
    spectacularAI::daiPlugin::Session* session = sai_depthai_pipeline_start_session(pipeline, nullptr);

    int counter = 0;
    while (counter < 1000) {
        if (sai_depthai_session_has_output(session)) {
            ++counter;
            VioOutputWrapper* output = sai_depthai_session_get_output(session);
            spectacularAI::Pose pose = sai_vio_output_get_pose(output);
            std::cout << counter << ". position = " << pose.position.x << ", " << pose.position.y << ", " << pose.position.z << std::endl;
            sai_vio_output_release(output); // must release memory!
        }
    }

    sai_depthai_session_release(session); // must release memory!
    sai_depthai_pipeline_release(pipeline); // must release memory!

    return 0;
}
